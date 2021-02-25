Feature: Options
    As a Course API consumer
    I want to retrieve standards and their options
    So that I can use them in my own application

@tag1
Scenario: Get list of active standards with options data
	Given I have an http client
	When I GET the following url: /api/courses/standards/options
	Then an http status code of 200 is returned
    And all valid and invalid standards and their options are returned
