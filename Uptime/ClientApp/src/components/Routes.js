import React, { Component } from 'react';
import { Dropdown } from 'semantic-ui-react';
import axios from 'axios';
import { Link } from 'react-router-dom';

class Routes extends Component {
  static displayName = Routes.name;

  constructor(props) {
    super(props);
    this.state = {
      destinations: [], loading: true, from: '', to: '', options: [{ key: '', value: '', text: '' }], routes: [], loading2: true,
      companies: [], possibleCompanies: [], currentSort: 'default'
    };

    this.handleDropdownChangeFrom = this.handleDropdownChangeFrom.bind(this);
    this.handleDropdownChangeTo = this.handleDropdownChangeTo.bind(this);
    this.handleClick = this.handleClick.bind(this);
    this.handleFilterClick = this.handleFilterClick.bind(this);
    //this.onSortChange = this.onSortChange.bind(this);
  }

  componentDidMount() {
    this.populateDestinationsData();
  }

  static renderRoutesTable(routes) {

    var newProviders = [];
    routes.map(({ travelRoutes, providers }) =>
      providers.map(provider => {
        provider.travelroutes = travelRoutes;
        newProviders.push(provider)
      })
    );


    return (
      <div className="container-fluid">
        <table className='table table-striped'>
          <thead>
            <tr>
              <th>Travel path</th>
              <th>Total distance</th>
              <th>Total price</th>
              <th>Travel start</th>
              <th>Travel end</th>
              <th>Total travel time</th>
              <th>Companies</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {
              newProviders.map(provider =>
                <tr key={provider.id}>
                  <td>{provider.travelroutes.map(travelRoute => <text>{travelRoute}  </text>)}</td>
                  <td>{provider.totalDistance}</td>
                  <td>{provider.totalPrice}</td>
                  <td>{provider.travelStart}</td>
                  <td>{provider.travelEnd}</td>
                  <td>{provider.totalTime.days} days {provider.totalTime.hours} hours</td>
                  <td>{provider.companies.map(company => <p>{company}</p>)}</td>
                  <td>
                    <Link role="button" to={{
                      pathname: '/CreateReservation',
                      totalDistance: provider.totalDistance,
                      totalPrice: provider.totalPrice,
                      travelStart: provider.travelStart,
                      travelEnd: provider.travelEnd,
                      totalTime: provider.totalTime,
                      companies: provider.companies,
                      travelRoutes: provider.travelroutes
                    }}>Create reservation</Link>
                  </td>
                </tr>
              )
            }
          </tbody>
        </table>
      </div>
    );
  }

  render() {


    let dropdowns = this.state.loading
      ? <p><em>Loading...</em></p>
      : <div>
        <div className="row row-margin">
          <div className="column column-width50">
            <Dropdown placeholder='From' fluid search selection
              onChange={this.handleDropdownChangeFrom}
              options={this.state.options}
            />
          </div>
          <div className="column column-width50">
            <Dropdown placeholder='To' fluid search selection
              onChange={this.handleDropdownChangeTo}
              options={this.state.options}
            />
          </div>
          <button className="btn" onClick={this.handleClick}>Submit</button>
        </div>
      </div>;

    const routes = this.state.routes;
    const currentSort = this.state.currentSort;
    var newProviders = [];
    routes.map(({ travelRoutes, providers }) =>
      providers.map(provider => {
        provider.travelroutes = travelRoutes;
        newProviders.push(provider)
      })
    );

    const sortTypes = {
      totalDistance_Up: {
        class: 'sort-up',
        fn: (a, b) => a.totalDistance - b.totalDistance
      },
      totalDistance_Down: {
        class: 'sort-down',
        fn: (a, b) => b.totalDistance - a.totalDistance
      },
      totalPrice_Up: {
        class: 'sort-up',
        fn: (a, b) => a.totalPrice - b.totalPrice
      },
      totalPrice_Down: {
        class: 'sort-down',
        fn: (a, b) => b.totalPrice - a.totalPrice
      },
      totalTime_Up: {
        class: 'sort-up',
        fn: (a, b) => a.totalTime.totalHours - b.totalTime.totalHours
      },
      totalTime_Down: {
        class: 'sort-down',
        fn: (a, b) => b.totalTime.totalHours - a.totalTime.totalHours
      },
      default: {
        class: 'sort',
        fn: (a, b) => a
      }
    };


    let dataTable = this.state.loading2
      ? <p></p>
      : <div className="container-fluid">
        <table className='table table-striped'>
          <thead>
            <tr>
              <th>Travel path</th>
              <th>
                <button className="btn" onClick={(e) =>this.onSortChange('totalDistance_')} 
                >
                  Total distance</button>
              </th>
              <th>
                <button className="btn" onClick={(e) =>this.onSortChange('totalPrice_')}>
                  Total price</button>
              </th>
              <th>Travel start</th>
              <th>Travel end</th>
              <th>
                <button className="btn" onClick={(e) =>this.onSortChange('totalTime_')}>
                  Total travel time</button>
              </th>
              <th>Companies</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {
              [...newProviders].sort(sortTypes[currentSort].fn).map(provider =>
                <tr key={provider.id}>
                  <td>{provider.travelroutes.map(travelRoute => <text>{travelRoute}  </text>)}</td>
                  <td>{provider.totalDistance}</td>
                  <td>{provider.totalPrice}</td>
                  <td>{provider.travelStart}</td>
                  <td>{provider.travelEnd}</td>
                  <td>{provider.totalTime.days} days {provider.totalTime.hours} hours</td>
                  <td>{provider.companies.map(company => <p>{company}</p>)}</td>
                  <td>
                    <Link role="button" to={{
                      pathname: '/CreateReservation',
                      validUntil: routes[0].validUntil,
                      totalDistance: provider.totalDistance,
                      totalPrice: provider.totalPrice,
                      travelStart: provider.travelStart,
                      travelEnd: provider.travelEnd,
                      totalTime: provider.totalTime,
                      companies: provider.companies,
                      travelRoutes: provider.travelroutes
                    }}>Create reservation</Link>
                  </td>
                </tr>
              )
            }
          </tbody>
        </table>
      </div>;

    let filtering = this.state.loading2
      ? <p></p>
      :
      <div>
        <h2>Filter offers by companies</h2>
        <form>
          <div>
            {this.state.possibleCompanies.map(company =>
              <div class="custom-control custom-checkbox">
                <input type="checkbox" class="custom-control-input" id={company} value={company} onChange={this.handleCheckboxChange} />
                <label class="custom-control-label" for={company}>{company} </label>
              </div>
            )}
          </div>
        </form>
        <button className="btn" onClick={this.handleFilterClick}>Filter</button>
      </div>;

    return (
      <div>
        <h1 id="tabelLabel" >Go to your dream vacation right now</h1>
        <p>Choose route from to where you want to travel.</p>
        {dropdowns}
        {filtering}
        {dataTable}
      </div>
    );
  }


  onSortChange = (header) => {

    let currentSort  = this.state.currentSort;
    let nextSort;
    if(currentSort !== "default")
    {currentSort = currentSort.split('_').pop();}


    if(currentSort=== 'Down') nextSort='Up';
    else if(currentSort === 'Up') nextSort = 'Down';
    else if(currentSort === 'default') nextSort = 'Up';

    nextSort = header + nextSort
    this.setState({
      currentSort: nextSort
    });
  };

  async populateDestinationsData() {
    const response = await fetch('api/PriceLists');
    const data = await response.json();
    this.setState({ destinations: data, loading: false });
    const gg = data;
    var newStateArray = [];
    gg.map((route) => {
      newStateArray.push({ value: route, text: route });
    });

    this.setState({ options: newStateArray })
  }

  handleDropdownChangeFrom(e, { value }) {
    this.setState({ from: value });
  }

  handleDropdownChangeTo(e, { value }) {
    this.setState({ to: value });
  }

  async handleClick() {
    if (this.state.from !== '' && this.state.to !== '' && this.state.to !== this.state.from) {
      const formData = [this.state.from, this.state.to];
      const response = await axios.post('/api/RouteInfos/destinations', formData)
        .catch(function (error) {
          console.log(error);
        });
      const data = response.data;
      var companies = [];
      data.map(route => route.companies.map(company => {
        if (!companies.includes(company)) {
          companies.push(company);
        }
      }))
      this.setState({ routes: data, loading2: false, companies: companies, possibleCompanies: companies });
    }
  }

  handleCheckboxChange = (event) => {
    const value = event.target.value;
    if (event.target.checked) {
      this.setState(prevState => ({ companies: prevState.companies.filter(day => day !== value) }));
    } else {
      if (!this.state.companies.includes(value)) {
        this.setState(prevState => ({ companies: [...prevState.companies, value] }))
      }
    }
  }

  async handleFilterClick() {

    const formData = {
      companies: this.state.companies,
      startAndEnd: [this.state.from, this.state.to]
    };
    const response = await axios.post('/api/RouteInfos/filter', formData)
      .catch(function (error) {
        console.log(error);
      });
    const data = response.data;

    this.setState({ routes: data});
  }

}
export default Routes;