#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development

./scripts/gen-backend-ts.sh

parcel --dist-dir=wwwroot/ web/index.html &
dotnet run &
wait
