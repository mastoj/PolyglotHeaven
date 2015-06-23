#!/bin/bash

docker stop eventstore
docker rm eventstore
docker stop elasticsearch
docker rm elasticsearch
docker stop neo4j
docker rm neo4j

docker build -t mastoj/elastic-marvel elasticsearch/
docker build -t mastoj/eventstore eventstore/
docker build -t mastoj/neo4j neo4j/
