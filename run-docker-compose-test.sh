#!/bin/bash
set -e
docker compose build

echo "##########################################"
echo "::notice ::Running unit tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-unit-tests.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from unit-tests

echo "##########################################"
echo "::notice ::Running integration tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-integration-tests.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from integration-tests
  
echo "##########################################"
echo "::notice ::Running system tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-system-tests.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from system-tests
  
echo "##########################################"
echo "::notice ::Running acceptance tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-acceptance-tests.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from acceptance-tests