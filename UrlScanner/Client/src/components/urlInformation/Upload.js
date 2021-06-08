import React, {useState, useRef} from 'react';
import api from '../../api';

const Upload = () => {
    const [file, setFile] = useState(null);
    const inputRef = useRef();

    const onChangeHandler = e => {
        setFile(e.target.files[0])
    };

    const onClickHandler = async () => {
        await api.uploads.uploadCsv(file);
        setFile(null);
        inputRef.current.value = ""
    }

    return (
        <React.Fragment>
            <input
                type="file"
                ref={inputRef}
                className="form-control-file"
                name="file"
                onChange={onChangeHandler}
            />
            <button
                type="button"
                disabled={!file}
                className="btn btn-success mt-1"
                onClick={onClickHandler}
            >Upload
            </button>
        </React.Fragment>
    );
};

export default Upload;