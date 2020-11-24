import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { Segment, Item, Image, List, Table, Button, Input, Dropdown } from 'semantic-ui-react';
import axios from 'axios';


const Wrapper = styled('div')`
	margin: 0 200px;
`;

const CollectionItemPage = (props) => {
	console.log(props);
	let itemId = props.match.params.id;

	const getSetImageLink = (number) => `https://img.bricklink.com/ItemImage/SN/0/${number}-1.png`;

	let [data, setData] = useState({
		item: undefined,
		parts: []
	});

	const loadContent = async () => {
		let itemResponse = await axios.get(`https://localhost:44390/api/collection/${itemId}`);
		let partsResponse = await axios.get(`https://localhost:44390/api/collection/${itemId}/parts`);


		setData({
			item: itemResponse.data,
			parts: partsResponse.data
		});
	}

	useEffect(() => {
		loadContent();
	}, []);

	let [filter, setFilter] = useState({
		number: '',
		color: '',
		name: ''
	});

	const changeColorFilter = (value) => {
		setFilter({
			...filter,
			color: value
		});
	}

	let [sort, setSort] = useState({
		column: '',
		direction: ''
	});

	const changeSorting = (col) => () => {
		if (col !== sort.column) {
			setSort({
				column: col,
				direction: 'ascending'
			});
			setData((prev) => ({
				item: prev.item,
				parts: prev.parts.sort(function(a,b) {
					if (a[col] < b[col]) return -1;
					if (a[col] > b[col]) return 1;
					return 0;
				})
			}));

			return
		}

		setSort((prev) => ({
			column: col,
			direction: prev.direction === 'ascending' ? 'descending' : 'ascending'
		}));
		setData((prev) => ({
			...prev,
			parts: prev.parts.reverse()
		}));
	}

	let [editInfo, setEditInfo] = useState({
		id: undefined,
		part: undefined
	});

	const handlePartEdit = (field) => (e, { value }) => {
		setEditInfo(prev => ({
			...prev,
			part: {
				...prev.part,
				[field]: value
			}
		}))
	}

	const openEditing = (id) => () => {
		let part = data.parts.find(p => p.id === id);

		setEditInfo({
			id,
			part
		});
	}

	const closeEditing = () => {
		setEditInfo({
			id: undefined,
			part: undefined
		});
	}

	const saveChanges = async () => {
		await axios.put(
			`https://localhost:44390/api/collection/${itemId}?num=0`,
			editInfo.part
		);

		await loadContent();

		closeEditing();
	}

	return (
		<Wrapper>
			<Segment>
				<Item.Group>
					{data.item && <Item>
						<Item.Image size='small' src={getSetImageLink(data.item.number)} />

						<Item.Content>
							<Item.Header as='h1'>Set {data.item.number}</Item.Header>
							<Item.Meta>{data.item.name}</Item.Meta>
							<Item.Description>
								<List>
									<List.Item>
										<List.Header>Цена</List.Header>
										<List.Content>{data.item.price}</List.Content>
									</List.Item>
									<List.Item>
										<List.Header>Доставка</List.Header>
										<List.Content>{data.item.toCityDeliveryPrice + data.item.toDoorDeliveryPrice}</List.Content>
									</List.Item>
								</List>
							</Item.Description>
						</Item.Content>
					</Item>}
				</Item.Group>
			</Segment>
			<Table
				celled
				sortable
				textAlign='center'
			>
				<Table.Header>
					<Table.Row>
						<Table.HeaderCell>
							Изображение
						</Table.HeaderCell>
						<Table.HeaderCell
							sorted={sort.column === 'number' ? sort.direction : null}
							onClick={changeSorting('number')}
						>
							Номер
						</Table.HeaderCell>
						<Table.HeaderCell
							sorted={sort.column === 'color' ? sort.direction : null}
							onClick={changeSorting('color')}
						>
							Цвет
						</Table.HeaderCell>
						<Table.HeaderCell
							sorted={sort.column === 'condition' ? sort.direction : null}
							onClick={changeSorting('condition')}
						>
							Состояние
						</Table.HeaderCell>
						<Table.HeaderCell
							sorted={sort.column === 'name' ? sort.direction : null}
							onClick={changeSorting('name')}
						>
							Название
						</Table.HeaderCell>
						<Table.HeaderCell
							sorted={sort.column === 'quantity' ? sort.direction : null}
							onClick={changeSorting('quantity')}
						>
							Количество
						</Table.HeaderCell>
						<Table.HeaderCell />
					</Table.Row>
				</Table.Header>
				<Table.Body>
					<Table.Row>
						<Table.Cell>
						</Table.Cell>
						<Table.Cell>
							<Input fluid />
						</Table.Cell>
						<Table.Cell>
							<Dropdown
								selection
								fluid
								options={[
									{ value: 'Red', text: 'Red' },
									{ value: 'Black', text: 'Black' },
								]}
								onChange={(e, d) => changeColorFilter(d.value)}
							/>
						</Table.Cell>
						<Table.Cell>
							<Dropdown
								selection
								fluid
								options={[
									{ value: 'Good', text: 'Good' },
									{ value: 'Poor', text: 'Poor' },
								]}
								onChange={(e, d) => changeColorFilter(d.value)}
							/>
						</Table.Cell>
						<Table.Cell>
							<Input fluid />
						</Table.Cell>
						<Table.Cell>

						</Table.Cell>
						<Table.Cell>

						</Table.Cell>
					</Table.Row>
					{data.parts && data.parts.map(part => (
						<Table.Row key={part.id}>
							<Table.Cell>
								<Image
									src={part.imageUrl}
									centered
									size='tiny'
								/>
							</Table.Cell>
							<Table.Cell>
								{part.number}
							</Table.Cell>
							<Table.Cell>
								{part.color}
							</Table.Cell>
							<Table.Cell>
								{part.condition}
							</Table.Cell>
							<Table.Cell>
								<b>{part.name}</b>
							</Table.Cell>
							<Table.Cell>
								{part.id !== editInfo.id
									? part.quantity
									: <Input
										value={editInfo.part.quantity}
										onChange={handlePartEdit('quantity')}
									/>
								}
							</Table.Cell>
							<Table.Cell>
								{part.id !== editInfo.id
									? <Button
										icon='pencil'
										onClick={openEditing(part.id)}
									/>
									: <>
										<Button
											icon='save'
											onClick={saveChanges}
										/>
										<Button
											icon='delete'
											onClick={closeEditing}
										/>
									</>
								}
								
							</Table.Cell>
						</Table.Row>
					))}
				</Table.Body>
			</Table>
		</Wrapper>
	);
}

export default CollectionItemPage;