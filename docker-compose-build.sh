#!/bin/bash
docker compose -f docker-compose-unit.yml build
docker compose -f docker-compose-integration.yml build
docker compose -f docker-compose-acceptance.yml build
