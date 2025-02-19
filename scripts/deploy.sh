#!/bin/bash
set -e

AWS_REGION="us-east-1"
AWS_ACCOUNT_ID="471112539374"
REPO_NAME="leaderboard-api"
IMAGE_TAG="latest"

echo "Starting deployment of ${REPO_NAME} to AWS ECR..."

echo "Authenticating to AWS ECR..."
aws ecr get-login-password --region ${AWS_REGION} | docker login --username AWS --password-stdin ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com

echo "Building Docker image..."
docker build --no-cache -t ${REPO_NAME} -f scripts/Dockerfile .

echo "Tagging Docker image..."
docker tag ${REPO_NAME}:latest ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com/${REPO_NAME}:${IMAGE_TAG}

echo "Pushing Docker image to AWS ECR..."
docker push ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com/${REPO_NAME}:${IMAGE_TAG}

echo "Deployment complete. Your image is now available in ECR."
