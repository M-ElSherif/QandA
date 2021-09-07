/** @jsxImportSource @emotion/react */
import { css } from '@emotion/react';
import React from 'react';
import { Provider } from 'react-redux';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { fontFamily, fontSize, gray2 } from '../CSS/Styles';
import { HomePage } from '../Pages/HomePage';
import { QuestionPage } from '../Pages/QuestionPage';
import { SearchPage } from '../Pages/SearchPage';
import { SignInPage } from '../Pages/SignInPage';
import { configureStore } from '../Store';
import { Header } from './Header';

const AskPage = React.lazy(() => import('../Pages/AskPage'));

const store = configureStore();

function App() {
  return (
    <Provider store={store}>
      <BrowserRouter>
        <div
          css={css`
            font-family: ${fontFamily};
            font-size: ${fontSize};
            color: ${gray2};
          `}
        >
          <Header />
          <Switch>
            <Route path="/search">
              <SearchPage />
            </Route>
            <Route path="/ask">
              <React.Suspense
                fallback={
                  <div
                    css={css`
                      margin-top: 100px;
                      text-align: center;
                    `}
                  >
                    Loading...
                  </div>
                }
              >
                <AskPage />
              </React.Suspense>
            </Route>
            <Route path="/signin">
              <SignInPage />
            </Route>
            <Route path="/questions/:questionId">
              <QuestionPage />
            </Route>
            <Route exact path="/">
              <HomePage />
            </Route>
          </Switch>
        </div>
      </BrowserRouter>
    </Provider>
  );
}

export default App;
