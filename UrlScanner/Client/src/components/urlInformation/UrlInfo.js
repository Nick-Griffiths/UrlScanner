import React from 'react'

const UrlInfo = props => {
    const urlInfo = props.urlInfo;

    return (
        <tr key={urlInfo.id}>
            <td>{urlInfo.name}</td>
            <td>{urlInfo.url}</td>
            <td>{urlInfo.hasGoogle}</td>
            <td>{urlInfo.phoneNumbers}</td>
            <td>{urlInfo.scanDurationInMilliseconds}</td>
            <td>{urlInfo.lastTimeScanned}</td>
        </tr>
    )
}

export default UrlInfo;