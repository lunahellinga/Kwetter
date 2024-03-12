import http from 'k6/http';
import exec from 'k6/execution';
import {SharedArray} from 'k6/data'
import {check} from 'k6';

const SERVICE = 'localhost:5002'
const BASE_URL = `http://${SERVICE}/api/Kweet`;
const PAYLOAD_FILE = '../Payloads/payload.json';
const posts = new SharedArray('posts', function () {
    return JSON.parse(open(PAYLOAD_FILE)).Posts
});
const updates = new SharedArray('updates', function () {
    return JSON.parse(open(PAYLOAD_FILE)).Updates
});
const gets = new SharedArray('gets', function () {
    return JSON.parse(open(PAYLOAD_FILE)).Gets
});

const VU = 100
const MAXDUR_SEC = 60
const LEN = 30000
export const options = {
    discardResponseBodies: true,
    scenarios: {
        post_scenario: {
            executor: 'shared-iterations',
            exec: 'post_func',
            vus: VU,
            iterations: LEN,
            maxDuration: `${MAXDUR_SEC}s`,
            startTime: "0s",
        },
        update_scenario: {
            executor: 'shared-iterations',
            exec: 'update_func',
            vus: VU,
            iterations: LEN,
            maxDuration: `${MAXDUR_SEC}s`,
            startTime: `${MAXDUR_SEC}s`,
        },
        get_scenario: {
            executor: 'shared-iterations',
            exec: 'get_func',
            vus: VU,
            iterations: LEN,
            maxDuration: `${MAXDUR_SEC}s`,
            startTime: `${MAXDUR_SEC * 2}s`,
        },
        get_1_scenario: {
            executor: 'shared-iterations',
            exec: 'get_100_func',
            vus: VU,
            iterations: LEN,
            maxDuration: `${MAXDUR_SEC}s`,
            startTime: `${MAXDUR_SEC * 3}s`,
        },
        delete_scenario: {
            executor: 'shared-iterations',
            exec: 'del_func',
            vus: VU,
            iterations: LEN,
            maxDuration: `${MAXDUR_SEC}s`,
            startTime: `${MAXDUR_SEC * 4}s`,
        }
    },
};


export function post_func() {
    const payload_data = posts[exec.scenario.iterationInTest];
    const res = http.post(BASE_URL, JSON.stringify(payload_data), {
        headers: {'Content-Type': 'application/json'},
    }, {tags: {post_tag: 'Post Request'}});
    check(res,
        {'POST /api/kweet status is 201': (r) => r.status === 201,},
        {post_tag: 'Post 201'}
    );
}

export function update_func() {
    const payload_data = updates[exec.scenario.iterationInTest];
    const res = http.put(BASE_URL, JSON.stringify(payload_data), {
        headers: {'Content-Type': 'application/json'},
    }, {tags: {update_tag: 'Update equest'}});
    check(res,
        {'PUT /api/kweet status is 200': (r) => r.status === 200,},
        {update_tag: 'Update 200'});
}

export function get_func() {
    const payload_data = gets[exec.scenario.iterationInTest];
    const res = http.get(`${BASE_URL}/${payload_data}`, {
        responseType: 'text',
        tags: {get1_tag: 'GetSingle Request'}
    });
    check(res,
        {'GET /api/kweet?id=x status is 200': (r) => r.status === 200,},
        {get1_tag: 'GetSingle 200'});
    check(res,
        {'GET /api/kweet contains message': (r) => r.body.includes('message'),},
        {get1_tag: 'GetSingle Contains'});
}

export function get_100_func() {
    const res = http.get(BASE_URL, {
        responseType: 'text',
        tags: {get100_tag: 'Get100 Request'}
    });
    check(res,
        {'GET /api/kweet status is 200': (r) => r.status === 200,},
        {get100_tag: 'Get100 200'});
    check(res,
        {'GET /api/kweet has multiple kweets': (r) => r.body.length > 10,},
        {get100_tag: 'Get100 Contains'});
}

export function del_func() {
    const payload_data = gets[exec.scenario.iterationInTest];
    const res = http.del(`${BASE_URL}/${payload_data}`, {
        tags: {delete_tag: 'Delete Request'}
    });
    check(res,
        {'DELETE /api/kweet status is 200': (r) => r.status === 200,},
        {delete_tag: 'Delete 200'});
}
