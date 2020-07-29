Feature: Standards
	As a Courses API consumer
    I want to retrieve standards
    So that I can use them in my own application

Scenario: Get list of standards
	Given I have an http client
    When I GET the following url: /api/courses/standards
    Then an http status code of 200 is returned
    And all valid standards are returned

    Scenario: Get list of standards by keyword
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title       | level | sector                        |
    | Brewer      | 1     | Engineering and manufacturing |
    | Head Brewer | 2     | Engineering and manufacturing |

    Scenario: Get list of standards by levels
	Given I have an http client
    When I GET the following url: /api/courses/standards?levels=1&levels=7
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title   | level | sector                        |
    | Brewer  | 1     | Engineering and manufacturing |
    | Dentist | 7     | Construction                  |

    Scenario: Get list of standards by sectors
	Given I have an http client
    When I GET the following url: /api/courses/standards?routeIds=B30D7750-9ADF-41BA-94BD-E4584128EC76
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                                           | level | sector       |
    | Dentist                                         | 7     | Construction |
    | Senior / head of facilities management (degree) | 6     | Construction |

    Scenario: Get list of standards by keyword and levels
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer&levels=1
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title       | level | sector                        |
    | Brewer      | 1     | Engineering and manufacturing |

    Scenario: Get list of standards by keyword sorted by relevance
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=sortorder&orderBy=score
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                            | level | sector              |
    | Junior animator SortOrder        | 4     | Creative and design |
    | Photographic assistant SortOrder | 3     | Creative and design |
    | Camera prep technician           | 3     | Creative and design |

    Scenario: Get list of standards by keyword sorted by name
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=sortorder&orderBy=title
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                            | level | sector              |
    | Camera prep technician           | 3     | Creative and design |
    | Junior animator SortOrder        | 4     | Creative and design |
    | Photographic assistant SortOrder | 3     | Creative and design |
    