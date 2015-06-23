#!/bin/bash

eval "$(docker-machine env)"

docker start elasticsearch
docker start neo4j
docker start eventstore
