import axios from "axios";

const defaultAxiosConfig = {
    baseURL: process.env.REACT_APP_API_URL!,
    headers: {
        'Content-Type': 'application/json',
    }
}

export const httpClient = axios.create(defaultAxiosConfig);
