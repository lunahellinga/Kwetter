```shell
k6 run \
    ./load_test.js
```

```shell
K6_CLOUD_TOKEN=f34275d0962cdcef1e52b9ab8c6ada3740b8ab28387ade2e2b3bba5652c3ba71
K6_CLOUD_PROJECT_ID=3644150

k6 run \
   --out json=./result.json \
   --out cloud \
    ./load_test.js

```