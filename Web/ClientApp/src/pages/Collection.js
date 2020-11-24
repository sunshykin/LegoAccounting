import React, { useState, useEffect } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { Table, TableHeaderCell, Image, Button, Modal, Header, Input } from 'semantic-ui-react'
import styled from 'styled-components';
import axios from 'axios';


const ButtonSection = styled('div')`
	display: flex;
	justify-content: flex-end;
`;

const ImageWithCursor = styled(Image)`
	cursor: pointer;
`;

const CollectionPage = () => {

	const getSetImageLink = (number) => `https://img.bricklink.com/ItemImage/SN/0/${number}-1.png`;
	const getSetLink = (number) => `https://www.bricklink.com/v2/catalog/catalogitem.page?S=${number}-1`;


	let location = useLocation();
	let history = useHistory();

	const openCollectionItem = (id) => {
		history.push(`/collection/${id}`);
	}

	const formatPrice = (price) => {
		return price && `${price} ₽`;
	}

	let content = [
		{
			id: '5f035147221b7b2810e51501',
			number: '8070',
			status: 'Получен',
			price: 4500,
			toCityDeliveryPrice: 505,
			toDoorDeliveryPrice: 25,
		},
		{
			id: '5f035147221b7b2810e51502',
			number: '42008',
			status: 'Получен',
			price: 2900,
			toCityDeliveryPrice: 485 / 2,
			toDoorDeliveryPrice: 16.59,
		},
		{
			id: '5f035147221b7b2810e51503',
			number: '42025',
			status: 'Получен',
			price: 2900,
			toCityDeliveryPrice: 485 / 2,
			toDoorDeliveryPrice: 16.59,
		},
		{
			id: '5f035147221b7b2810e51504',
			number: '42099',
			status: 'Получен',
			price: 7000,
			toCityDeliveryPrice: 514,
			toDoorDeliveryPrice: 40.05,
		},
		{
			id: '5f035147221b7b2810e51505',
			number: '42024',
			status: 'Отправлен',
			price: 1000,
			toCityDeliveryPrice: 392 / 2,
			toDoorDeliveryPrice: null,
		},
		{
			id: '5f035147221b7b2810e51506',
			number: '42050',
			status: 'Отправлен',
			price: 1000,
			toCityDeliveryPrice: 392 / 2,
			toDoorDeliveryPrice: null,
		}
	];

	let [data, setData] = useState();

	const loadContent = async () => {
		let response = await axios.get('https://localhost:44390/api/collection/list');

		setData(response.data);
	}

	useEffect(() => {
		console.log('useEffect called');
		loadContent()
		/*(async function dummyLoad() {
			await loadContent();
		})();*/
	}, []);

	let [modalOpen, setModalOpen] = useState(false);

	const toggleModal = () => {
		setModalOpen(value => !value);
	}

	let [inputValue, setInputValue] = useState();

	const handleInputChange = (e, { value }) => {
		setInputValue(value);
	}

	const createCollectionItem = async () => {
		await axios.post('https://localhost:44390/api/collection', { setNumber: inputValue });
		await loadContent();
		toggleModal();
	}

	const deleteCollectionItem = (id) => async () => {
		await axios.delete(`https://localhost:44390/api/collection/${id}`);
		await loadContent();
	}

	return (
		<>
			<Modal
				open={modalOpen}
				onClose={toggleModal}
				size='small'
			>
				<Modal.Header>Добавить новый объект в коллекцию</Modal.Header>
				<Modal.Content>
					<p>Введите номер набора для добавления</p>
					<Input
						placeholder="Номер набора"
						onChange={handleInputChange}
						value={inputValue}
					/>
				</Modal.Content>
				<Modal.Actions>
					<Button
						color='red'
						onClick={toggleModal}
					>
						Закрыть
					</Button>
					<Button
						color='green'
						onClick={createCollectionItem}
					>
						Сохранить
					</Button>
				</Modal.Actions>
			</Modal>
			<ButtonSection>
				<Button
					icon='plus'
					title='Добавить новый элемент в коллекцию'
					onClick={toggleModal}
				/>
			</ButtonSection>
			{data && <Table
				striped
				celled
				textAlign='center'
			>
				<Table.Header>
					<Table.Row>
						<TableHeaderCell content='Изображение' />
						<TableHeaderCell content='Модель' />
						<TableHeaderCell content='Статус' />
						<TableHeaderCell content='Ссылка' />
						<TableHeaderCell content='Цена' />
						<TableHeaderCell content='Цена доставки до города' />
						<TableHeaderCell content='Цена доставки до двери' />
						<TableHeaderCell />
					</Table.Row>
				</Table.Header>
				<Table.Body>
					{data.map(item => (
						<Table.Row key={item.id}>
							<Table.Cell>
								<ImageWithCursor
									src={getSetImageLink(item.number)}
									centered
									size='small'
									onClick={() => openCollectionItem(item.id)}
								/>
							</Table.Cell>
							<Table.Cell>
								{item.number}
							</Table.Cell>
							<Table.Cell>
								<b>
									{item.status}
								</b>
							</Table.Cell>
							<Table.Cell>
								<Button
									onClick={() => window.open(getSetLink(item.number), '_blank')}
								>
									BrickLink
								</Button>
							</Table.Cell>
							<Table.Cell>
								{formatPrice(item.price)}
							</Table.Cell>
							<Table.Cell>
								{formatPrice(item.toCityDeliveryPrice)}
							</Table.Cell>
							<Table.Cell>
								{formatPrice(item.toDoorDeliveryPrice)}
							</Table.Cell>
							<Table.Cell>
								<Button 
									icon='delete'
									onClick={deleteCollectionItem(item.id)}
								/>
							</Table.Cell>
						</Table.Row>
					))}
				</Table.Body>
			</Table>}
		</>
	);
}

export default CollectionPage;