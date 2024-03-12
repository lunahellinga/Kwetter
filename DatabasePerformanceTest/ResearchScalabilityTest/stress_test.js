import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    ext: {
        loadimpact: {
          projectID: 3642900,
          name: "Research_Scalability"
        }
      },
    stages: [
        { duration: '2m', target: 400},
        { duration: '4m', target: 400},
        { duration: '4m', target: 0}
    ],
};

export default function () {
    http.batch([
        ['GET', 'http://localhost:5050/KweetRead/AllKweets'],
        ['GET', 'http://localhost:5050/KweetRead/ReactionsByKweet?kweetId=1476cc1f-bbee-431e-93aa-7d9898aaed78']
    ])
    sleep(1)
}