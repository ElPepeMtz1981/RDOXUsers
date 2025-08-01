#!/bin/bash
set -e

echo "Start after_install.sh" >> /home/ubuntu/rdoxusers.log

# Exporta las variables para usar el SDK correcto
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$HOME/.dotnet:$PATH

sudo chown -R ubuntu:ubuntu /home/ubuntu/rdoxuserssc

chmod -R u+rwX /home/ubuntu/rdoxuserssc

echo "change directory publish" >> /home/ubuntu/rdoxusers.log

# 👉 Navega al código fuente
cd /home/ubuntu/rdoxuserssc

echo "start publish" >> /home/ubuntu/rdoxusers.log
# 👉 Publica en la carpeta de artefactos
$DOTNET_ROOT/dotnet publish RDOXLogin.csproj -c Release -o /home/ubuntu/rdoxusers

echo "End publish" >> /home/ubuntu/rdoxusers.log

echo "End after_install.sh." >> /home/ubuntu/rdoxusers.log