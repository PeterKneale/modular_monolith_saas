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

  if [ $? -ne 0 ]; then
    echo "::error file=${compose_file},line=1,col=1::${test_type^} tests failed"
    exit 1
  fi
}

echo "::notice ::Running tests..."
run_tests "unit"
run_tests "integration"
run_tests "system"
run_tests "acceptance"
echo "::notice ::Running tests completed."