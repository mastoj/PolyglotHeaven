#!/bin/bash

eval "$(docker-machine env)"

docker run -d --name=elasticsearch -p 9200:9200 -p 9300:9300 mastoj/elastic-marvel
docker run -d --name=neo4j -p 7474:7474 mastoj/neo4j
docker run -d --name=eventstore -p 2113:2113 -p 1113:1113 mastoj/eventstore
