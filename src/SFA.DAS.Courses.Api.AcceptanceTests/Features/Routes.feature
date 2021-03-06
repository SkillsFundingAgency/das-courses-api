﻿Feature: Routes
	As a Courses API consumer
    I want to retrieve routes
    So that I can use them in my own application

Scenario: Get list of routes
	Given I have an http client
    When I GET the following url: /api/courses/routes
    Then an http status code of 200 is returned
    And all routes are returned
