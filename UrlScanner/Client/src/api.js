import axios from "axios";

const instance = axios.create({
    baseURL: process.env.REACT_APP_API_URL
});

instance.defaults.headers.common.Accept = 'application/json';
instance.defaults.headers.common['Content-Type'] = 'application/json';

const api = {
    urlInfos: {
        get() {
            return instance.get("urlInfos", );
        },
        getLastFullScan() {
            return instance.get("urlInfos/lastFullScan");
        }
    },
    uploads: {
        uploadCsv(file) {
            return instance.post("uploads", file, {
                headers: {
                    "Content-Type": "text/csv"
                }
            })
        }
    }
}

export default api