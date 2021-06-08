import React, { useEffect, useState } from 'react';
import api from '../../api';

const LastFullScan = () => {
  const [loading, setLoading] = useState(true);
  const [lastFullScan, setLastFullScan] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      const response = await api.urlInfos.getLastFullScan();
      setLastFullScan(response.data.lastFullScan);
      setLoading(false);
    };
    fetchData();
  }, []);

  return (
    (!loading) && <h4 className='float-right'>Last Full Scan Completed: {lastFullScan}</h4>
  );
};

export default LastFullScan;