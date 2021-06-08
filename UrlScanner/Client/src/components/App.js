import React from 'react';
import {Route} from 'react-router';
import UrlInfos from './urlInformation/UrlInfos';
import Upload from './urlInformation/Upload';
import Layout from './layout/Layout';
import '../custom.css';

const App = () => {
    return (
        <Layout>
            <Route exact path='/' component={UrlInfos}/>
            <Route path='/upload' component={Upload}/>
        </Layout>
    )
}

export default App