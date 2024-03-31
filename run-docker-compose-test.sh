#!/bin/bash
set -e

echo "##########################################"
echo "Running unit tests"
echo "{Running unit tests}" >> $GITHUB_STEP_SUMMARY
echo "##########################################"
docker compose --progress quiet -f docker-compose-unit.yml build
docker compose --progress quiet -f docker-compose-unit.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from unit-tests

echo "##########################################"
echo "Running integration tests"
echo "{Running integration Tests}" >> $GITHUB_STEP_SUMMARY
echo "##########################################"
docker compose --progress quiet -f docker-compose-integration.yml build
docker compose --progress quiet -f docker-compose-integration.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from integration-tests
  
echo "##########################################"
echo "Running acceptance tests"
echo "{Running acceptance Tests}" >> $GITHUB_STEP_SUMMARY
echo "##########################################"
docker compose --progress quiet -f docker-compose-acceptance.yml build
docker compose --progress quiet -f docker-compose-acceptance.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from acceptance-tests