docker:
	docker compose rm -f && docker compose up

mongo:
	docker run -it --rm \
		--user $UID \
		-v $(pwd)/Scripts:/scripts \
		-v $(pwd)/Payloads:/payloads \
		-v $(pwd)/TestResults:/jsonoutput \
		--network=databaseperformancetest_performance-test \
		grafana/k6 run --out json=/jsonoutput/result_mongo.json /scripts/k6_tests_mongo.js

neo:
	docker run -it --rm \
		--user $UID \
		-v $(pwd)/Scripts:/scripts \
		-v $(pwd)/Payloads:/payloads \
		-v $(pwd)/TestResults:/jsonoutput \
		--network=databaseperformancetest_performance-test \
		grafana/k6 run --out json=/jsonoutput/result_neo4j.json /scripts/k6_tests_neo4j.js

postgres:
	docker run -it --rm \
		--user $UID \
		-v $(pwd)/Scripts:/scripts \
		-v $(pwd)/Payloads:/payloads \
		-v $(pwd)/TestResults:/jsonoutput \
		--network=databaseperformancetest_performance-test \
		grafana/k6 run --out json=/jsonoutput/result_postgres.json /scripts/k6_tests_postgres.js

yugabyte:
	docker run -it --rm \
		--user $UID \
		-v $(pwd)/Scripts:/scripts \
		-v $(pwd)/Payloads:/payloads \
		-v $(pwd)/TestResults:/jsonoutput \
		--network=databaseperformancetest_performance-test \
		grafana/k6 run --out json=/jsonoutput/result_yuga.json /scripts/k6_tests_yuga.js

tests: mongo neo postgres yugabyte