/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import {
  FieldContainer,
  FieldError,
  FieldLabel,
  Fieldset,
  FieldTextArea,
  FormButtonContainer,
  gray3,
  gray6,
  PrimaryButton,
  SubmissionSuccess,
} from '../CSS/Styles';
import React from 'react';
import { useParams } from 'react-router-dom';
import { Page } from './Page';
import {
  getQuestion,
  postAnswer,
  QuestionData,
  mapQuestionFromServer,
  QuestionDataFromServer,
} from '../Components/QuestionsData';
import { AnswerList } from '../Components/AnswerList';
import { useForm } from 'react-hook-form';
import {
  HubConnectionBuilder,
  HubConnectionState,
  HubConnection,
} from '@aspnet/signalr';

type FormData = {
  content: string;
};

export const QuestionPage = () => {
  const [successfullySubmitted, setSuccessfullySubmitted] =
    React.useState(false);

  const {
    register,
    formState: { errors },
    handleSubmit,
    formState,
  } = useForm<FormData>({ mode: 'onBlur' });

  const [question, setQuestion] = React.useState<QuestionData | null>(null);

  // method to setup a SignalR connection
  const setUpSignalRConnection = async (questionId: number) => {
    // TODO - setup connection to real-time SignalR API
    const connection = new HubConnectionBuilder()
      .withUrl('https://localhost:44354/questionshub')
      .withAutomaticReconnect()
      .build();
    // TODO - handle Message function being called
    connection.on('Message', (message: string) => {
      console.log('Message', message);
    });
    // TODO - handle ReceiveQuestion function being called
    connection.on('ReceiveQuestion', (question: QuestionDataFromServer) => {
      console.log('ReceiveQuestion', question);
      setQuestion(mapQuestionFromServer(question));
    });
    // TODO - start the connection
    try {
      await connection.start();
    } catch (err) {
      console.log(err);
    }
    // TODO - subscribe to question
    if (connection.state === HubConnectionState.Connected) {
      connection
        .invoke('SubscribeQuestion', Number(questionId))
        .catch((err: Error) => {
          return console.error(err.toString());
        });
    }
    // TODO - return the connection
    return connection;
  };

  // method to clean up SignalR connection
  const cleanUpSignalRConnection = async (
    questionId: number,
    connection: HubConnection,
  ) => {
    // TODO - unsubscribe from the question
    if (connection.state === HubConnectionState.Connected) {
      try {
        await connection.invoke('UnsubscribeQuestion', Number(questionId));
      } catch (err) {
        return console.log('error');
      }
      connection.off('Message');
      connection.off('ReceiveQuestion');
      connection.stop();
    } else {
      // TODO - stop the connection
      connection.off('Message');
      connection.off('ReceiveQuestion');
      connection.stop();
    }
  };

  const { questionId }: any = useParams();

  const doGetQuestion = async (questionId: number) => {
    const foundQuestion = await getQuestion(questionId);
    setQuestion(foundQuestion);
  };

  React.useEffect(() => {
    let connection: HubConnection;
    if (questionId) {
      doGetQuestion(Number(questionId));
      setUpSignalRConnection(questionId).then((con) => {
        connection = con;
      });
    }

    return function cleanUp() {
      if (questionId) {
        cleanUpSignalRConnection(Number(questionId), connection);
      }
    };
  }, [questionId]);

  const submitForm = async (data: FormData) => {
    const result = await postAnswer({
      questionId: question!.questionId,
      content: data.content,
      userName: 'Fred',
      created: new Date(),
    });
    setSuccessfullySubmitted(result ? true : false);
  };

  return (
    <Page>
      <div
        css={css`
          background-color: white;
          padding: 15px 20px 20px 20px;
          border-radius: 4px;
          border: 1px solid ${gray6};
          box-shadow: 0 3px 5px 0 rgba(0, 0, 0, 0.16);
        `}
      >
        <div
          css={css`
            font-size: 19px;
            font-weight: bold;
            margin: 10px 0px 5px;
          `}
        >
          {question === null ? '' : question.title}
        </div>
        {question !== null && (
          <React.Fragment>
            <p
              css={css`
                margin-top: 0px;
                background-color: white;
              `}
            >
              {question.content}
            </p>
            <div
              css={css`
                font-size: 12px;
                font-style: italic;
                color: ${gray3};
              `}
            >
              {`Asked by ${question.userName} on
            ${question.created.toLocaleDateString()}
            ${question.created.toLocaleTimeString()}`}
            </div>
            <AnswerList data={question.answers} />
            <form
              onSubmit={handleSubmit(submitForm)}
              css={css`
                margin-top: 20px;
              `}
            >
              <Fieldset
                disabled={formState.isSubmitting || successfullySubmitted}
              >
                <FieldContainer>
                  <FieldLabel htmlFor="content">Your Answer</FieldLabel>
                  <FieldTextArea
                    {...register('content', { required: true, minLength: 50 })}
                    id="content"
                    name="content"
                  />
                  {errors.content && errors.content.type === 'required' && (
                    <FieldError>You must enter the answer</FieldError>
                  )}
                  {errors.content && errors.content.type === 'minLength' && (
                    <FieldError>
                      The answer must be at least 50 characters
                    </FieldError>
                  )}
                </FieldContainer>
                <FormButtonContainer>
                  <PrimaryButton type="submit">
                    Submit Your Answer
                  </PrimaryButton>
                </FormButtonContainer>
                {successfullySubmitted && (
                  <SubmissionSuccess>
                    Your answer was successfully submitted
                  </SubmissionSuccess>
                )}
              </Fieldset>
            </form>
          </React.Fragment>
        )}
      </div>
    </Page>
  );
};
