/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import { useLocation, useParams } from 'react-router-dom';

import React from 'react';
import { Page } from './Page';
import { QuestionList } from '../Components/QuestionList';
import { QuestionData, searchQuestions } from '../Components/QuestionsData';

export const SearchPage = () => {
  // Get the search parameters in the URL
  const SearchParams = () => {
    return new URLSearchParams(useLocation().search);
  };
  const [questions, setQuestions] = React.useState<QuestionData[]>([]);
  const search = SearchParams().get('criteria') || '';

  React.useEffect(() => {
    const doSearch = async (criteria: string) => {
      const foundResults = await searchQuestions(criteria);
      setQuestions(foundResults);
    };
    doSearch(search);
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
