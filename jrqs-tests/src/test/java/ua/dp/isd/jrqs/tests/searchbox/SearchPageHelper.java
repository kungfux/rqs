package ua.dp.isd.jrqs.tests.searchbox;

import java.util.ArrayList;
import java.util.List;

import org.openqa.selenium.WebElement;
import ua.dp.isd.jrqs.tests.WebHelper;

public class SearchPageHelper {
    
    private final String URL = "http://localhost/jrqs";
    private final WebHelper wh = new WebHelper();
    
    private final String cssSearchInput = "#value";
    private final String cssSearchButton = "button[type='submit']";

    public void doSearchBy(String text) {
        wh.getDriver().get(URL);
        wh.findElementByCss(cssSearchInput).clear();
        wh.findElementByCss(cssSearchInput).sendKeys(text);
        wh.findElementByCss(cssSearchButton).click();
    }
    
    public String[] getSearchResultsInColumn(int column) {
        List<String> results = new ArrayList<>();
        wh.findElementsByCss(String.format("table tr td:nth-child(%s)", column)).forEach((e) -> {
            results.add(e.getText());
        });
        return results.toArray(new String[results.size()]);
    }
       
    public void clickToShareTheRequirementRow(int row) {
        List<WebElement> els = wh.findElementsByCss("a[title='Share']");
        if (els.size() >= row)  {
            els.get(row-1).click();
        } else {
            throw new IllegalStateException("There is no Share link to click in the row #" + row);
        }
    }
    
    public void clickToRepeatSearchByTmsTaskInRow(int row) {
        List<WebElement> els = wh.findElementsByCss("table td:nth-child(3) a");
        if (els.size() >= row)  {
            els.get(row-1).click();
        } else {
            throw new IllegalStateException("There is no TMS Task link to click in the row #" + row);
        }
    }
    
    public void clickToFilterOutBySourceInRow(int row) {
        List<WebElement> els = wh.findElementsByCss("table td:nth-child(12) a");
        if (els.size() >= row)  {
            els.get(row-1).click();
        } else {
            throw new IllegalStateException("There is no Limit By Source link to click in the row #" + row);
        }
    }
    
    public Boolean isFilterOutBySourceLinksAvailable() {
        return wh.findElementsByCssWithoutAwaiting("table td:nth-child(12) a").size() > 0;
    }
}
