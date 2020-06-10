Feature: Standards
	As a Courses API consumer
    I want to retrieve standards
    So that I can use them in my own application

@mytag
Scenario: Get list of standards
	Given I have an http client
    When I GET the following url: /api/courses/standards
    Then an http status code of 200 is returned
