/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import { useLocation, useParams } from 'react-router-dom';

import React from 'react';
import { Page } from './Page';
import { QuestionList } from '../Components/QuestionList';
import { searchQuestions } from '../Components/QuestionsData';
import { useSelector, useDispatch } from 'react-redux';
import {
  AppState,
  searchingQuestionsAction,
  searchedQuestionsAction,
} from '../Store';

export const SearchPage = () => {
  const dispatch = useDispatch();
  const questions = useSelector((state: AppState) => state.questions.searched);
  // Get the search parameters in the URL
  const SearchParams = () => {
    return new URLSearchParams(useLocation().search);
  };
  const search = SearchParams().get('criteria') || '';

  React.useEffect(() => {
    const doSearch = async (criteria: string) => {
      dispatch(searchingQuestionsAction());
      const foundResults = await searchQuestions(criteria);
      dispatch(searchedQuestionsAction(foundResults));
    };
    doSearch(search);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [search]);

  return (
    <Page title="Search Results">
      {search && (
        <p
          css={css`
            font-size: 16px;
            font-style: italic;
            margin-top: 0px;
          `}
        >
          for "{search}"
        </p>
      )}
      <QuestionList data={questions} />
    </Page>
  );
};
