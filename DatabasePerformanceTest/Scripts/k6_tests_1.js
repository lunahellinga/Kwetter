import http from 'k6/http';
import exec from 'k6/execution';
import {SharedArray} from 'k6/data'
import {check} from 'k6';

const SERVICE = 'neo4j-service'
const BASE_URL = `http://${SERVICE}/api/Kweet`;
const PAYLOAD_FILE = '/payloads/payload_1.json';
const posts = new SharedArray('posts', function () {
    return JSON.parse(open(PAYLOAD_FILE)).Posts
});
const updates = new SharedArray('updates', function () {
    return JSON.parse(open(PAYLOAD_FILE)).Updates
});
const gets = new SharedArray('gets', function () {
    return JSON.parse(open(PAYLOAD_FILE)).Gets
});

export const options = {
    discardResponseBodies: true,
    scenarios: {
        post_scenario: {
            executor: 'shared-iterations',
            exec: 'post_func',
            vus: 1,
            iterations: 1,
            maxDuration: '1s',
            startTime: "0s",
        },
        update_scenario: {
            executor: 'shared-iterations',
            exec: 'update_func',
            vus: 1,
            iterations: 1,
            maxDuration: '1s',
            startTime: "1s",
        },
        get_scenario: {
            executor: 'shared-iterations',
            exec: 'get_func',
            vus: 1,
            iterations: 1,
            maxDuration: '1s',
            startTime: "2s",
        },
        get_1_scenario: {
            executor: 'shared-iterations',
            exec: 'get_100_func',
            vus: 1,
            iterations: 1,
            maxDuration: '1s',
            startTime: "3s",
        },
        delete_scenario: {
            executor: 'shared-iterations',
            exec: 'del_func',
            vus: 1,
            iterations: 1,
            maxDuration: '1s',
            startTime: "4s",
        }
    },
};


export function post_func() {
    const payload_data = posts[exec.scenario.iterationInTest];
    const res = http.post(BASE_URL, JSON.stringify(payload_data), {
        headers: {'Content-Type': 'application/json'},
    });
    check(res, {
        'POST /api/kweet status is 201': (r) => r.status === 201,
    });
    console.log(res)
}

export function update_func() {
    const payload_data = updates[exec.scenario.iterationInTest];
    const res = http.put(BASE_URL, JSON.stringify(payload_data), {
        headers: {'Content-Type': 'application/json'},
    });
    check(res, {
        'PUT /api/kweet status is 200': (r) => r.status === 200,
    });
    console.log(res)
}

export function get_func() {
    const payload_data = gets[exec.scenario.iterationInTest];
    const res = http.get(`${BASE_URL}/${payload_data}`, {
        responseType: 'text',
    });
    check(res, {
        'GET /api/kweet?id=x status is 200': (r) => r.status === 200,
    });
    check(res, {
        'GET /api/kweet contains message': (r) => r.body.includes('message'),
    });
    console.log(res)
}

export function get_100_func() {
    const res = http.get(BASE_URL, {
        responseType: 'text',
    });
    check(res, {
        'GET /api/kweet status is 200': (r) => r.status === 200,
    });
    check(res, {
        'GET /api/kweet has multiple kweets': (r) => r.body.length > 10,
    });
    console.log(res)
}

export function del_func() {
    const payload_data = gets[exec.scenario.iterationInTest];
    const res = http.del(`${BASE_URL}/${payload_data}`);
    check(res, {
        'DELETE /api/kweet status is 200': (r) => r.status === 200,
    });
    console.log(res)
}
