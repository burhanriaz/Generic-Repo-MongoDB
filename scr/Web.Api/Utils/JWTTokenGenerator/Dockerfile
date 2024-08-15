FROM ubuntu:20.04
RUN apt-get update && apt-get install -y python3.9
EXPOSE 443
COPY . .
CMD ["python3.9", "main.py"]