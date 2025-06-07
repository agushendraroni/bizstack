#!/bin/bash

echo "Pilih folder untuk rebuild Docker images:"
echo "1) services"
echo "2) frontend"
echo "3) all"
read -p "Masukkan pilihan (1, 2, atau 3): " choice

folders=()

case "$choice" in
  1)
    folders=("services")
    ;;
  2)
    folders=("frontend")
    ;;
  3)
    folders=("services" "frontend")
    ;;
  *)
    echo "Pilihan tidak valid. Exit."
    exit 1
    ;;
esac

for basefolder in "${folders[@]}"; do
  if [ ! -d "$basefolder" ]; then
    echo "Folder '$basefolder' tidak ditemukan. Lewati."
    continue
  fi

  echo "Membangun Docker images di folder '$basefolder'..."

  # Cari semua Dockerfile di bawah basefolder secara rekursif
  find "$basefolder" -type f -name Dockerfile | while read -r dockerfile; do
    dir=$(dirname "$dockerfile")
    echo "Building image for $dir"
    docker build -t "$(basename "$dir")-image" "$dir"
    echo "Finished building $(basename "$dir")-image"
  done
done

echo "Selesai."
