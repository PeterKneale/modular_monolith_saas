#!/bin/bash

run_tests() {
  local test_type=$1
  local compose_file="docker-compose-${test_type}-tests.yml"

  echo "::notice ::Running ${test_type} tests"
  docker compose --progress quiet -f "${compose_file}" up \
    --force-recreate \
    --remove-orphans \
    --no-log-prefix \
    --abort-on-container-exit \
    --exit-code-from "${test_type}-tests"
}

echo "::notice ::Running tests..."
run_tests "unit"
run_tests "integration"
run_tests "system"
run_tests "acceptance"
echo "::notice ::Running tests completed."