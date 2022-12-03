FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine

ARG PUBLISH_FOLDER_NAME="publish"
ARG APP_FOLDER="app"
ARG ENTRY_POINT

ENV app_folder=$APP_FOLDER
ENV entry_point=$ENTRY_POINT

RUN mkdir -p $APP_FOLDER 
COPY $PUBLISH_FOLDER_NAME/$APP_FOLDER ./$APP_FOLDER

CMD ["sh", "-c", "cd ${app_folder} && dotnet ${entry_point}"]