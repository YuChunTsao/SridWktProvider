#!/bin/bash

echo "Generating SRID to WKT mapping CSV..."

projinfo --list-crs --authority EPSG | awk '{print $1}' | while read -r code; do
  authority=${code%%:*}
  code_num=${code#*:}
  wkt=$(projinfo -q -o WKT1:GDAL --single-line "$code")
  wkt_escaped=$(printf '%s' "$wkt" | sed 's/"/""/g')
  printf '%s,%s,"%s"\n' "$authority" "$code_num" "$wkt_escaped"
done | {
  echo "Authority,Code,Wkt"
  cat
} >srid_wkt_mapping.csv

mkdir -p data
mv srid_wkt_mapping.csv ./data/srid_wkt_mapping.csv

echo "CSV generation completed: srid_wkt_mapping.csv"
