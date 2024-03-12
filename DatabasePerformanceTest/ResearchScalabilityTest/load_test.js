import http from 'k6/http';
import {sleep, check} from 'k6';
import {uuidv4} from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';

const SERVICE = "router"
const BASE_URL = "kind.cluster/"
const URL = `https://${SERVICE}.${BASE_URL}`

export const options = {
    ext: {
        loadimpact: {
            projectID: 3644116,
            name: "Research_Scalability"
        }
    },
    stages: [
        {duration: '1m', target: 50},
        {duration: '2m', target: 50},
        {duration: '1m', target: 0}
    ],
};

export default function () {
    const randomUUID = uuidv4();
    const res = http.post(`${URL}raw`, JSON.stringify({
        "vehicleId": randomUUID,
        "coordinates": [
            {
                "lat": 1,
                "lon": 1,
                "time": "2023-06-04T20:06:30.838Z"
            },
            {
                "lat": 2,
                "lon": 2,
                "time": "2023-06-04T20:06:33.838Z"
            },
            {
                "lat": 3,
                "lon": 3,
                "time": "2023-06-04T20:06:36.838Z"
            },
            {
                "lat": 4,
                "lon": 4,
                "time": "2023-06-04T20:06:39.838Z"
            },
            {
                "lat": 5,
                "lon": 5,
                "time": "2023-06-04T20:06:42.838Z"
            }
        ]
    }), {
        headers: {'Content-Type': 'application/json'},
    })
    sleep(1)
}