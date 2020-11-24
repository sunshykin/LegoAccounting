import React from 'react';
import { Route, Switch } from 'react-router';
import styled, {  } from 'styled-components'

import { Home } from './pages/Home';
import CollectionPage from './pages/Collection';
import CollectionItemPage from './pages/CollectionItem';

const AppWrapper = styled('div')`
	margin: 30px;
`;

const App = () => {

	return (
		<AppWrapper>
			<Switch>
				<Route
					exact
					path='/'
				>
					<Home />
				</Route>
				<Route
					exact
					path='/collection'
				>
					<CollectionPage />
				</Route>
				<Route
					exact
					path='/collection/:id'
					component={CollectionItemPage}
				/>
			</Switch>
		</AppWrapper>
	);
}

export default App;