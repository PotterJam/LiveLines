import React, {useEffect, useState} from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export function NavMenu(props) {
  const [collapsed, setCollapsed] = useState(true);
  const toggleNavbar = () => setCollapsed(!collapsed);
  
  const [loggedIn, setLoggedIn] = useState(false);

  useEffect(() => {
    const checkIfLoggedIn = async () =>
    {
      const response = await fetch('api/authenticated');
      setLoggedIn(response.ok);
    }
    checkIfLoggedIn();
  }, []);

  const loginWithGithub = `api/login?returnUrl=${window.location.origin}/`;

  const loginOrProfile = loggedIn => {
    return loggedIn
      ? <span>Logged in!</span>
      : <a href={loginWithGithub} className="btn btn-outline-secondary btn-sm" role="button">
          <i className="bi bi-github" /> &nbsp; Login with Github
        </a>
  }
  
  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <Container className="d-sm-inline-flex">
          <NavbarBrand tag={Link} to="/">LiveLines</NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
              </NavItem>
              <NavItem className="ms-2 row align-items-center">
                {loginOrProfile(loggedIn)}
              </NavItem>
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
}
