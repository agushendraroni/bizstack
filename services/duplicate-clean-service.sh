#!/bin/bash

# Detect available services
SERVICE_DIR="./"
SERVICE_LIST=($(find "$SERVICE_DIR" -maxdepth 1 -type d -name "*-service" -exec basename {} \; | sort))

echo "📚 Available source services:"
select SRC in "${SERVICE_LIST[@]}"; do
  if [[ -n "$SRC" ]]; then
    break
  else
    echo "❌ Invalid selection. Please choose a valid service."
  fi
done

# Ask destination (auto remove -service suffix)
read -p "📝 Destination (e.g. hr): " DEST_SHORT

DEST="${DEST_SHORT}-service"
SRC_SHORT="${SRC%-service}"
SRC_NAME="$(tr '[:lower:]' '[:upper:]' <<< ${SRC_SHORT:0:1})${SRC_SHORT:1}Service"
DEST_NAME="$(tr '[:lower:]' '[:upper:]' <<< ${DEST_SHORT:0:1})${DEST_SHORT:1}Service"

# Detect .csproj file
SRC_CSPROJ_FILE=$(find "$SRC" -name "*.csproj" | head -n 1)
if [[ -z "$SRC_CSPROJ_FILE" ]]; then
  echo "❌ No .csproj found in $SRC"
  exit 1
fi
SRC_CSPROJ_FILENAME=$(basename "$SRC_CSPROJ_FILE")
DEST_CSPROJ_FILENAME="${DEST}.csproj"

# Show summary
echo "📦 Source folder: $SRC"
echo "📦 Destination folder: $DEST"
echo "📛 Source .csproj: $SRC_CSPROJ_FILENAME"
echo "📛 Destination .csproj: $DEST_CSPROJ_FILENAME"

# Solution path
DEFAULT_SOLUTION_PATH="../BizStack.sln"
read -p "🛠 Path to solution file (default: $DEFAULT_SOLUTION_PATH): " SLN_PATH
SLN_PATH="${SLN_PATH:-$DEFAULT_SOLUTION_PATH}"

read -p "✅ Proceed? (y/n): " CONFIRM
[[ "$CONFIRM" != "y" ]] && echo "❌ Canceled." && exit 1

# Copy project
echo "📁 Copying project..."
cp -r "$SRC" "$DEST"

# Rename .csproj
echo "🔄 Renaming .csproj file..."
mv "$DEST/$SRC_CSPROJ_FILENAME" "$DEST/$DEST_CSPROJ_FILENAME"

# Replace namespace references
echo "🔧 Updating namespace references..."
grep -rl "$SRC_NAME" "$DEST" | xargs sed -i "s/$SRC_NAME/$DEST_NAME/g"

# Rename DbContext
SRC_CONTEXT_CLASS="${SRC_SHORT^}DbContext"
DEST_CONTEXT_CLASS="${DEST_SHORT^}DbContext"
SRC_CONTEXT_FILE=$(find "$DEST" -type f -name "${SRC_CONTEXT_CLASS}.cs" | head -n 1)

if [[ -n "$SRC_CONTEXT_FILE" ]]; then
  echo "🛠 Renaming DbContext class from $SRC_CONTEXT_CLASS to $DEST_CONTEXT_CLASS"
  sed -i "s/$SRC_CONTEXT_CLASS/$DEST_CONTEXT_CLASS/g" "$DEST"/*.cs "$DEST"/*/*.cs "$DEST"/*/*/*.cs
  mv "$SRC_CONTEXT_FILE" "${SRC_CONTEXT_FILE%/*}/${DEST_CONTEXT_CLASS}.cs"
fi

# Clean domain folders
echo "🧹 Cleaning domain folders..."
rm -rf "$DEST/Models" "$DEST/DTOs" "$DEST/Controllers" "$DEST/Services" "$DEST/Validation"
mkdir -p "$DEST/Models" "$DEST/DTOs" "$DEST/Controllers"
mkdir -p "$DEST/Services/Interfaces" "$DEST/Services/Implementations"

# Add to solution
if [[ -f "$SLN_PATH" ]]; then
  echo "➕ Adding project to solution $SLN_PATH..."
  dotnet sln "$SLN_PATH" add "$DEST/$DEST_CSPROJ_FILENAME"
else
  echo "⚠️  Solution file not found at $SLN_PATH, skipping solution update."
fi

echo "✅ Done! '$DEST' is ready."
