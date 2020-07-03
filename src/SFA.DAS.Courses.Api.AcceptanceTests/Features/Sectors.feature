Feature: Sectors
	As a Courses API consumer
    I want to retrieve sectors
    So that I can use them in my own application

Scenario: Get list of sectors
	Given I have an http client
    When I GET the following url: /api/courses/sectors
    Then an http status code of 200 is returned
    Then all sectors are returned
