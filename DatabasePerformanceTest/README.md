```shell
docker compose build
```

```shell
docker compose rm -f && docker compose up
```

```shell
docker run -it --rm \
  --user $UID \
  -v $(pwd)/Scripts:/scripts \
  -v $(pwd)/Payloads:/payloads \
  -v $(pwd)/TestResults:/jsonoutput \
  --network=databaseperformancetest_performance-test \
  grafana/k6 run --out json=/jsonoutput/result_1.json /scripts/k6_tests_1.js

```

## Mongo

```shell
docker run -it --rm \
  --user $UID \
  -v $(pwd)/Scripts:/scripts \
  -v $(pwd)/Payloads:/payloads \
  -v $(pwd)/TestResults:/jsonoutput \
  --network=databaseperformancetest_performance-test \
  grafana/k6 run --out json=/jsonoutput/result_mongo.json /scripts/k6_tests_mongo.js

```

## Neo4J

```shell
docker run -it --rm \
  --user $UID \
  -v $(pwd)/Scripts:/scripts \
  -v $(pwd)/Payloads:/payloads \
  -v $(pwd)/TestResults:/jsonoutput \
  --network=databaseperformancetest_performance-test \
  grafana/k6 run --out json=/jsonoutput/result_neo4j.json /scripts/k6_tests_neo4j.js

```

## Postgres

```shell
docker run -it --rm \
  --user $UID \
  -v $(pwd)/Scripts:/scripts \
  -v $(pwd)/Payloads:/payloads \
  -v $(pwd)/TestResults:/jsonoutput \
  --network=databaseperformancetest_performance-test \
  grafana/k6 run --out json=/jsonoutput/result_postgres.json /scripts/k6_tests_postgres.js

```

## Yugabyte

```shell
docker run -it --rm \
  --user $UID \
  -v $(pwd)/Scripts:/scripts \
  -v $(pwd)/Payloads:/payloads \
  -v $(pwd)/TestResults:/jsonoutput \
  --network=databaseperformancetest_performance-test \
  grafana/k6 run --out json=/jsonoutput/result_yuga.json /scripts/k6_tests_yuga.js

```

## All

```shell
docker run -it --rm \
  --user $UID \
  -v $(pwd)/Scripts:/scripts \
  -v $(pwd)/Payloads:/payloads \
  -v $(pwd)/TestResults:/jsonoutput \
  --network=databaseperformancetest_performance-test \
  grafana/k6 run --out json=/jsonoutput/result_mongo.json /scripts/k6_tests_mongo.js
docker run -it --rm \
  --user $UID \
  -v $(pwd)/Scripts:/scripts \
  -v $(pwd)/Payloads:/payloads \
  -v $(pwd)/TestResults:/jsonoutput \
  --network=databaseperformancetest_performance-test \
  grafana/k6 run --out json=/jsonoutput/result_neo4j.json /scripts/k6_tests_neo4j.js
docker run -it --rm \
  --user $UID \
  -v $(pwd)/Scripts:/scripts \
  -v $(pwd)/Payloads:/payloads \
  -v $(pwd)/TestResults:/jsonoutput \
  --network=databaseperformancetest_performance-test \
  grafana/k6 run --out json=/jsonoutput/result_postgres.json /scripts/k6_tests_postgres.js
docker run -it --rm \
  --user $UID \
  -v $(pwd)/Scripts:/scripts \
  -v $(pwd)/Payloads:/payloads \
  -v $(pwd)/TestResults:/jsonoutput \
  --network=databaseperformancetest_performance-test \
  grafana/k6 run --out json=/jsonoutput/result_yuga.json /scripts/k6_tests_yuga.js

```

## Local

```shell
k6 run \
    ./Scripts/k6_tests_mongo.js

k6 run \
    ./Scripts/k6_tests_yuga.js

k6 run \
    ./Scripts/k6_tests_postgres.js

k6 run \
    ./Scripts/k6_tests_neo4j.js
```

## To cloud


```shell
K6_CLOUD_TOKEN=f34275d0962cdcef1e52b9ab8c6ada3740b8ab28387ade2e2b3bba5652c3ba71
K6_CLOUD_PROJECT_ID=3644150

k6 run \
   --out json=./TestResults/result_mongo.json \
   --out cloud \
    ./Scripts/k6_tests_mongo.js

k6 run \
   --out json=./TestResults/result_yuga.json \
   --out cloud \
    ./Scripts/k6_tests_yuga.js

k6 run \
   --out json=./TestResults/result_postgres.json \
   --out cloud \
    ./Scripts/k6_tests_postgres.js

k6 run \
   --out json=./TestResults/result_neo4j.json \
   --out cloud \
    ./Scripts/k6_tests_neo4j.js
```