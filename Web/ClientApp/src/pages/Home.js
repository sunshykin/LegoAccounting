import React from 'react';
import { Link } from 'react-router-dom';
import { Header } from 'semantic-ui-react';

export const Home = () => {

	return (
		<div>
			<Header as='h1'>
				Hello, there, stranger!
			</Header>
			<p>We are here to introduce to you our project.</p>
			<Link to='/collection'>
				Go to the collection
			</Link>
		</div>
	);
}