import React from 'react';
import logo from '../../images/logo-colorido.png';
import './App.css';
import { Component } from 'react';
import axios from 'axios'
// var baseUrl = 'https://localhost:44308';

class Login extends Component{
	constructor(props) {
		super(props)

		this.state = {
			login: '',
			senha: ''
		}
	}	

	changeHandler = e =>{
		this.setState({[e.target.name]: e.target.value})
	}

	handleSubmit = e =>{
		e.preventDefault()
		console.log(this.state)	
		axios
			.post('https://localhost:44308/User/auth', this.state )
			.then(response =>{
				console.log(response)
			})	
			.catch(error => {
				console.log(error)
			})
	}

	render() {
		const{ login, senha } = this.state
		return (
			<div className="App">
				<div className="container-fluid">
					<a id="logo" className="navbar-brand center-block">
					<img src={logo} className="img-size"/>
					</a>
					<div className="Modal">
						<form
							className="ModalForm"
							onSubmit={this.handleSubmit}>	
							<div className="">				
								<input
									id="login"
									type="text"
									name="login"
									placeholder="e-mail"
									autoComplete="false"
									required
									className="Input"									
									onChange={this.changeHandler}/>	
							</div>					
							<div className="">
								<input
									id="senha"
									type="text"
									name="senha"
									placeholder="senha"
									autoComplete="false"									
									className="Input"									
									onChange={this.changeHandler}/>
							</div>						
							<button type="submit">
								Entrar <i className="fa fa-fw fa-chevron-right"></i>
							</button>
						</form>
					</div>
				</div>				        	
			</div>
		);
  }
}

export default Login;