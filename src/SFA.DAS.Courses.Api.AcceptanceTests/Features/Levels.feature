Feature: Levels
	As a Courses API consumer
    I want to retrieve levels
    So that I can use them in my own application

@mytag
Scenario: Get list of levels
	Given I have an http client
    When I GET the following url: /api/courses/levels
    Then an http status code of 200 is returned
    And all levels are returned
