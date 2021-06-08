import React, {useState, useEffect} from "react";
import UrlInfo from "./UrlInfo";
import LastFullScan from "./LastFullScan";
import api from '../../api';

const UrlInfos = () => {
    const [loading, setLoading] = useState(true);
    const [urlInfos, setUrlInfos] = useState([])

    useEffect(() => {
        fetchData()
    }, [])

    const fetchData = async () => {
        const response = await api.urlInfos.get();
        setUrlInfos(response.data);
        setLoading(false);
    }

    const refresh = async () => {
        setLoading(true);
        await fetchData();
    }

    const render = urlInfos => {
        return (
            <table className='table table-striped'>
                <thead>
                <tr>
                    <th>Name</th>
                    <th>URL</th>
                    <th>Has Google</th>
                    <th>Phone Numbers</th>
                    <th>Scan Duration (ms)</th>
                    <th>Last Time Scanned</th>
                </tr>
                </thead>
                <tbody>
                {urlInfos.map(urlInfo =>
                    <UrlInfo key={urlInfo.id} urlInfo={urlInfo}/>
                )}
                </tbody>
            </table>
        )
    }

    return (
        <div>
            <LastFullScan />
            <h1>URL Information</h1>
            <button className="btn btn-primary mb-1" onClick={refresh} disabled={loading}>Refresh</button>
            {loading
                ? <p><em>Loading...</em></p>
                : render(urlInfos)
            }
        </div>
    )
}

export default UrlInfos;