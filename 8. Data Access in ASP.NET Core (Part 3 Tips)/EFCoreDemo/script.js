import http from 'k6/http';
import { sleep } from 'k6';
export const options = {
    // runs a 30 second, 500 Virtual users(VUs) load test
    vus: 500,
    duration: '30s',
};

export default function () {
    // sends this GET request for 30 seconds
    http.get('http://localhost:5152/api/invoice');
    sleep(1);
}