#!/bin/bash
set -e

echo "Ejecutando start_server.sh..." >> /home/ubuntu/rdoxusers.log

# Restart the api service
sudo systemctl restart UsersService

echo "Servicio reiniciado correctamente." >> /home/ubuntu/rdoxusers.log