#!/bin/bash

./scripts/gen-backend-ts.sh
parcel build --dist-dir=wwwroot/ web/index.html
