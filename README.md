# LinkedIn Profile Photo Taker

This project is designed to automate the process of logging into LinkedIn and retrieving the profile photo of a user. The application is implemented in C# using Selenium WebDriver, and it is packaged in a Docker container for easy deployment and testing.

## Features

- **Automated Login:** automatically logs into LinkedIn using provided credentials
- **Profile Photo Extraction:** navigates to the profile page and retrieves the profile photo URL
- **CAPTCHA Bypass Strategy:** the CAPTCHA challenge is bypassed by simulating constant clicks on specific areas of the page. This approach helps to trigger the necessary events to load the profile photo despite the challenge
- **Dockerized Environment:** all dependencies are contained within the Docker container, ensuring a consistent and easy-to-deploy environment

## Usage

1. **Build the Docker Container:**

   ```bash
   docker-compose up --build
   ```

2. **Send a Request:**

   Make a POST request to `http://localhost:8080/api/profile-photo` with the required credentials in JSON format. For example:

   ```json
   {
     "Login": "your-email@example.com",
     "Password": "your-password"
   }
   ```

## CAPTCHA Bypass Method

The current strategy for bypassing CAPTCHA involves simulating constant mouse clicks on predefined areas of the page. These clicks help trigger the events required to load the profile photo, effectively circumventing the CAPTCHA challenge. While this method is effective in this controlled scenario.

## Logging

Logs are redirected to `out.log` for easy troubleshooting and monitoring.
