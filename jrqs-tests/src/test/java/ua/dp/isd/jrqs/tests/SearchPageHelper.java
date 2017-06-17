package ua.dp.isd.jrqs.tests;

import java.util.ArrayList;
import java.util.List;

import org.openqa.selenium.WebElement;

public class SearchPageHelper {
    
    private final String URL = "http://localhost/jrqs";
    private final WebHelper wh = new WebHelper();

    public void doSearch(String text) {
        wh.getDriverInstance().get(URL);
        
        wh.findElement("#value").clear();
        wh.findElement("#value").sendKeys(text);
        wh.findElement("button[type='submit']").click();
    }
    
    public String[] getSearchResultsByColumn(int column) {
        List<String> results = new ArrayList<>();
        for (WebElement e : wh.findElements(String.format("table tr td:nth-child(%s)", column))) {
            results.add(e.getText());
        }
        return results.toArray(new String[results.size()]);
    }
    
    public String[] doSearchByAndGetCellResults(String text, int column) {
        doSearch(text);
        return getSearchResultsByColumn(column);
    }
}
