ENV_FILE = .env

ENV_VARS = CERT_PATH=/etc/letsencrypt/live/boardly.ru/

.DEFAULT:
	@echo "Выполняется перед любой целью"

env:
	@$(eval SHELL:=/bin/bash)
	@printf "%s\n" $(ENV_VARS) > $(ENV_FILE)
	@echo "$(ENV_FILE) file created"

run:
	@chmod +x docker/scripts/entrypoint.sh
	@docker compose up --build -d

runl:
	@chmod +x docker/scripts/entrypoint.sh
	@docker compose up --build

off:
	@docker compose down

db:
	@docker compose up --build -d db pgbouncer

logs:
	@docker compose logs

deploy-run:
	@docker compose -f docker-compose.yaml up --build
