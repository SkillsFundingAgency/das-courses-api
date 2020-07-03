Feature: Standards
	As a Courses API consumer
    I want to retrieve standards
    So that I can use them in my own application

Scenario: Get list of standards
	Given I have an http client
    When I GET the following url: /api/courses/standards
    Then an http status code of 200 is returned
    Then all standards are returned

    Scenario: Get list of standards by keyword
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer
    Then an http status code of 200 is returned
    Then the following standards are returned
    | title       | level | sector                        |
    | Brewer      | 1     | Engineering and manufacturing |
    | Head Brewer | 2     | Engineering and manufacturing |

    Scenario: Get list of standards by levels
	Given I have an http client
    When I GET the following url: /api/courses/standards?levels=1&levels=7
    Then an http status code of 200 is returned
    Then the following standards are returned
    | title   | level | sector                        |
    | Brewer  | 1     | Engineering and manufacturing |
    | Dentist | 7     | Construction                  |

    Scenario: Get list of standards by sectors
	Given I have an http client
    When I GET the following url: /api/courses/standards?routeIds=B30D7750-9ADF-41BA-94BD-E4584128EC76
    Then an http status code of 200 is returned
    Then the following standards are returned
    | title                                           | level | sector       |
    | Dentist                                         | 7     | Construction |
    | Senior / head of facilities management (degree) | 6     | Construction |

    Scenario: Get list of standards by keyword and levels
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer&levels=1
    Then an http status code of 200 is returned
    Then the following standards are returned
    | title       | level | sector                        |
    | Brewer      | 1     | Engineering and manufacturing |
